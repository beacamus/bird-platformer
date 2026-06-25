shader_type canvas_item;

varying vec2 vertex_local;
uniform vec2 tile_size = vec2(64.0, 64.0);

void vertex() {
    vertex_local = VERTEX;
}

uniform sampler2D test_texture;
uniform vec4 top_left_region;     // x, y, width, height in UV space (0-1)
uniform vec4 top_right_region;
uniform vec4 bottom_left_region;
uniform vec4 bottom_right_region;

// Remaps a local 0-1 quadrant UV into a specific region of the atlas
vec2 remap_to_region(vec2 uv, vec4 region) {
    return region.xy + uv * region.zw;
}

void fragment() {
    vec2 local_uv = mod(vertex_local, tile_size) / tile_size;

    if (local_uv.x < 0.5 && local_uv.y < 0.5) {
        vec2 quadrant_uv = local_uv * 2.0; // 0-1 within this quadrant
        COLOR = texture(test_texture, remap_to_region(quadrant_uv, bottom_right_region));
    } else if (local_uv.x >= 0.5 && local_uv.y < 0.5) {
        vec2 quadrant_uv = vec2((local_uv.x - 0.5) * 2.0, local_uv.y * 2.0);
        COLOR = texture(test_texture, remap_to_region(quadrant_uv, bottom_left_region));
    } else if (local_uv.x < 0.5 && local_uv.y >= 0.5) {
        vec2 quadrant_uv = vec2(local_uv.x * 2.0, (local_uv.y - 0.5) * 2.0);
        COLOR = texture(test_texture, remap_to_region(quadrant_uv, top_right_region));
    } else {
        vec2 quadrant_uv = (local_uv - 0.5) * 2.0;
        COLOR = texture(test_texture, remap_to_region(quadrant_uv, top_left_region));
    }
}