function LoadTexture(image64, width, height) {
    var canvas = document.getElementById("canvas");
    var ctx = canvas.getContext("2d");
    var img = new Image();
    img.src = image64;
    img.onload = function () {
        ctx.drawImage(img, 0, 0, width, height);
    };
    img.onerror = function (ex) {
        console.log("Error: " + ex);
    };
}

function RenderImage(image64) {
    var canvas = document.getElementById("canvas");
    var gl = canvas.getContext("webgl");
    gl.clearColor(0.2, 0.2, 0.2, 1.0);
    gl.clear(gl.COLOR_BUFFER_BIT);
}

function SetupEvents(dotnetRef) {
    var canvas = document.getElementById("canvas");
    canvas.addEventListener('mousedown', function (evt) {
        dotnetRef.invokeMethodAsync('eventMouseDown', evt.offsetX, evt.offsetY);
        console.log("mousedown");
    }, false);
    canvas.addEventListener('mouseup', function (evt) {
        dotnetRef.invokeMethodAsync('eventMouseUp', evt.offsetX, evt.offsetY);
        console.log("mouseup");
    }, false);
    canvas.addEventListener('mousemove', function (evt) {
        dotnetRef.invokeMethodAsync('eventMouseMove', evt.offsetX, evt.offsetY);
        console.log("mousemove");
    }, false);
}