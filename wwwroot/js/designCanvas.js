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
