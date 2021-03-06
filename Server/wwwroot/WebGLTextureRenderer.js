var programInfo;

function Initialize(vertexShaderSource, fragmentShaderSource) {
    console.log("Initializing");
    const canvas = document.getElementById("canvas");
    const gl = canvas.getContext("webgl");
    console.log(gl);

    const vertexShader = createShader(gl, gl.VERTEX_SHADER, vertexShaderSource);
    const fragmentShader = createShader(gl, gl.FRAGMENT_SHADER, fragmentShaderSource);
    const shaderProgram = createProgram(gl, vertexShader, fragmentShader);
    gl.useProgram(shaderProgram);

    programInfo = {
        program: shaderProgram,
        webgl: gl,
        texture: initTexture(gl, canvas.width, canvas.height),
        buffers: {
            textureBuffer: createTextureBuffer(gl),
            positionBuffer: createVertexBuffer(gl)
        },
        attribLocations: {
            positionLocation: gl.getAttribLocation(shaderProgram, 'a_position'),
            textureCoord: gl.getAttribLocation(shaderProgram, 'a_texCoord'),
        },
        uniformLocations: {
            resolution: gl.getUniformLocation(shaderProgram, "u_resolution")
        }
    };
    console.log(programInfo);

    const size = 2;          // 2 components per iteration
    const type = gl.FLOAT;   // the data is 32bit floats
    const normalize = false; // don't normalize the data
    const stride = 0;        // 0 = move forward size * sizeof(type) each iteration to get the next position
    const offset = 0;        // start at the beginning of the buffer

    // set the resolution
    gl.uniform2f(programInfo.uniformLocations.resolution, gl.canvas.width, gl.canvas.height);

    // Turn on the textureCoord attribute
    // bind the textureCoord buffer.
    // Tell the textureCoord attribute how to get data out of textureBuffer (ARRAY_BUFFER)
    gl.enableVertexAttribArray(programInfo.attribLocations.textureCoord);
    gl.bindBuffer(gl.ARRAY_BUFFER, programInfo.buffers.textureBuffer);
    gl.vertexAttribPointer(programInfo.attribLocations.textureCoord, size, type, normalize, stride, offset);

    // Turn on the positionLocation attribute
    // bind the positionLocation buffer.
    // Tell the positionLocation attribute how to get data out of positionBuffer (ARRAY_BUFFER)
    gl.enableVertexAttribArray(programInfo.attribLocations.positionLocation);
    gl.bindBuffer(gl.ARRAY_BUFFER, programInfo.buffers.positionBuffer);
    gl.vertexAttribPointer(programInfo.attribLocations.positionLocation, size, type, normalize, stride, offset);

    console.log("Initialized");
}

function createVertexBuffer(gl) {
    console.log("Creating vertex buffer");
    const positionBuffer = gl.createBuffer();
    createVertexBufferData(gl, positionBuffer, 0, 0, gl.canvas.width, gl.canvas.height);
    return positionBuffer;
}

function createTextureBuffer(gl) {
    console.log("Creating texture buffer");
    const texcoordBuffer = gl.createBuffer();
    createTextureBufferData(gl, texcoordBuffer);
    return texcoordBuffer;
}

function SetShaderSources(vsSource, fsSource) {
    vertexShaderSource = vsSource;
    fragmentShaderSource = fsSource;
}

function initTexture(gl, width, height) {
    const tex = gl.createTexture();
    gl.bindTexture(gl.TEXTURE_2D, tex);

    const level = 0;
    const internalFormat = gl.RGB;
    const border = 0;
    const srcFormat = gl.RGB;
    const srcType = gl.UNSIGNED_BYTE;
    const pixel = new Uint8Array([0, 0, 255]);  // opaque blue
    gl.texImage2D(gl.TEXTURE_2D, level, internalFormat, width, height, border, srcFormat, srcType, pixel);

    // Turn off mips and set  wrapping to clamp to edge so it
    // will work regardless of the dimensions of the video.
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
    console.log("Initialized texture");
    return tex;
}

function draw() {
    const gl = programInfo.webgl;
    const offset = 0;
    const primitiveType = gl.TRIANGLES;
    const primitiveCount = 6;
    gl.drawArrays(primitiveType, offset, primitiveCount);
}

function updateTexture(video, width, height, redraw = true) {
    const gl = programInfo.webgl;
    const level = 0;
    const internalFormat = gl.RGB;
    const srcFormat = gl.RGB;
    const srcType = gl.UNSIGNED_BYTE;
    const border = 0;
    gl.bindTexture(gl.TEXTURE_2D, programInfo.texture);
    gl.texImage2D(gl.TEXTURE_2D, level, internalFormat, width, height, border, srcFormat, srcType, video);

    if(redraw)
        draw();
}

function createTextureBufferData(gl, buffer) {
    gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
        0.0, 0.0,
        1.0, 0.0,
        0.0, 1.0,
        0.0, 1.0,
        1.0, 0.0,
        1.0, 1.0,
    ]), gl.STATIC_DRAW);
    console.log("Initialized buffer data");
}

function createVertexBufferData(gl, buffer, x, y, width, height) {
    const x1 = x;
    const x2 = x + width;
    const y1 = y;
    const y2 = y + height;
    gl.bindBuffer(gl.ARRAY_BUFFER, buffer);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
        x1, y1,
        x2, y1,
        x1, y2,
        x1, y2,
        x2, y1,
        x2, y2,
    ]), gl.STATIC_DRAW);
    console.log("Initialized buffer data");
}

function createProgram(gl, vertexShader, fragmentShader) {
    const program = gl.createProgram();
    console.log(vertexShader);

    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);
    gl.linkProgram(program);

    const success = gl.getProgramParameter(program, gl.LINK_STATUS);
    if (success) {
        console.log("Initialized shader program");
        return program;
    }

    console.log(gl.getProgramInfoLog(program));
    gl.deleteProgram(program);
}

function createShader(gl, type, source) {
    const shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);

    const success = gl.getShaderParameter(shader, gl.COMPILE_STATUS);
    if (success) {
        console.log("Created shader: " + type);
        return shader;
    }

    gl.deleteShader(shader);
}

