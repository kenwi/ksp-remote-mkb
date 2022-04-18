attribute vec3 aPos;
attribute vec2 aTex;
varying vec2 vTex;
uniform mat4 u_matrix;

void main() {
    gl_Position = u_matrix * vec4(aPos, 1.0);
    vTex = aTex;
}
