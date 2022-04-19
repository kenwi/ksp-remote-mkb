function LoadTexture(image64) {
    var canvas = document.getElementById("canvas");
    var ctx = canvas.getContext("2d");
    var img = new Image();
    img.src = image64;
    img.onload = function () {
        ctx.drawImage(img, 0, 0, 800, 600);
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