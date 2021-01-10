import Color from 'color-js';

export function colorTest() {
    var green = Color("#00FF00");
    console.log(green);
    console.log(green.toHSV())
    console.log(green.getHue())
    return green.toString();
}


export function getHue(colorHex) {
    var color = Color(colorHex);
    return Math.trunc(color.getHue());
}

export function getSaturation(colorHex) {
    var color = Color(colorHex);
    return Math.trunc(color.getSaturation() * 100);
}

export function getLightness(colorHex) {
    var color = Color(colorHex);
    return Math.trunc(color.getLightness() * 100);
}

export function hslToHex(h, s, l) {
    var color = Color({ hue: h, saturation: s / 100, lightness: l / 100 });
    return color.toString();
}


export function getRed(colorHex) {
    var color = Color(colorHex);
    return Math.trunc(color.getRed() * 255);
}

export function getGreen(colorHex) {
    var color = Color(colorHex);
    return Math.trunc(color.getGreen() * 255);
}

export function getBlue(colorHex) {
    var color = Color(colorHex);
    return Math.trunc(color.getBlue() * 255);
}

export function rgbToHex(r, g, b) {
    var color = Color([r, g, b]);
    return color.toString();
}


export function setHue(colorHex, hue) {
    var color = Color(colorHex).setHue(hue);
    return color.toString();
}

export function setSaturation(colorHex, saturation) {
    var color = Color(colorHex).setSaturation(saturation / 100);
    return color.toString();
}

export function setLightness(colorHex, lightness) {
    var color = Color(colorHex).setLightness(lightness / 100);
    return color.toString();
}
