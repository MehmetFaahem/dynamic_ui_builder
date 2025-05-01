// Design Canvas JavaScript Module
window.designCanvas = {
  initialize: function (canvasElement, dotNetRef) {
    if (!canvasElement) return;

    // Store the .NET reference
    this.dotNetRef = dotNetRef;
    this.canvasElement = canvasElement;

    // Add mouse events to the canvas
    this.setupMouseEvents(canvasElement);

    // Add event listeners for the window to ensure mouse up is captured even if outside the canvas
    window.addEventListener("mouseup", this.handleMouseUp.bind(this));
    window.addEventListener("mousemove", this.handleMouseMove.bind(this));
  },

  setupMouseEvents: function (canvasElement) {
    canvasElement.addEventListener(
      "mousedown",
      this.handleMouseDown.bind(this)
    );
  },

  handleMouseDown: function (event) {
    if (!this.dotNetRef) return;

    const rect = this.canvasElement.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;

    this.dotNetRef.invokeMethodAsync("OnCanvasMouseDown", x, y);
  },

  handleMouseMove: function (event) {
    if (!this.dotNetRef || !this.canvasElement) return;

    const rect = this.canvasElement.getBoundingClientRect();

    // Check if the mouse is within the canvas bounds
    if (
      event.clientX >= rect.left &&
      event.clientX <= rect.right &&
      event.clientY >= rect.top &&
      event.clientY <= rect.bottom
    ) {
      const x = event.clientX - rect.left;
      const y = event.clientY - rect.top;

      this.dotNetRef.invokeMethodAsync("OnCanvasMouseMove", x, y);
    }
  },

  handleMouseUp: function () {
    if (!this.dotNetRef) return;

    this.dotNetRef.invokeMethodAsync("OnCanvasMouseUp");
  },

  dispose: function () {
    if (this.canvasElement) {
      this.canvasElement.removeEventListener("mousedown", this.handleMouseDown);
    }

    window.removeEventListener("mouseup", this.handleMouseUp);
    window.removeEventListener("mousemove", this.handleMouseMove);

    this.dotNetRef = null;
    this.canvasElement = null;
  },
};

// Design Canvas Drag and Drop JavaScript

let isDragging = false;
let currentElement = null;
let startX, startY;
let originalX, originalY;

// Initialize drag and drop functionality
function initDragDrop() {
  document.addEventListener("mouseup", endDrag);
  document.addEventListener("mousemove", drag);
}

// Start dragging a component
function startDrag(element, x, y, initialX, initialY) {
  isDragging = true;
  currentElement = element;
  startX = x;
  startY = y;
  originalX = initialX;
  originalY = initialY;

  // Add a dragging class for visual feedback
  element.classList.add("dragging");

  return true;
}

// Handle the drag operation
function drag(e) {
  if (!isDragging || !currentElement) return;

  // Calculate the new position
  const dx = e.clientX - startX;
  const dy = e.clientY - startY;

  // Set the new position
  currentElement.style.left = `${originalX + dx}px`;
  currentElement.style.top = `${originalY + dy}px`;
}

// End dragging when mouse is released
function endDrag() {
  if (!isDragging || !currentElement) return;

  // Remove the dragging class
  currentElement.classList.remove("dragging");

  // Extract the new position values
  const newX = parseInt(currentElement.style.left);
  const newY = parseInt(currentElement.style.top);

  // Get the component ID from data attribute
  const componentId = currentElement.getAttribute("data-component-id");

  // Call the .NET method to update the component's position
  DotNet.invokeMethodAsync(
    "UIBuilderApp",
    "UpdateComponentPosition",
    componentId,
    newX,
    newY
  );

  // Reset dragging state
  isDragging = false;
  currentElement = null;
}

// Initialize when the document is ready
document.addEventListener("DOMContentLoaded", initDragDrop);

// Export functions for Blazor interop
window.designCanvas = {
  startDrag,
  getElementBounds: (element) => {
    const rect = element.getBoundingClientRect();
    return {
      x: rect.left,
      y: rect.top,
      width: rect.width,
      height: rect.height,
    };
  },
};
