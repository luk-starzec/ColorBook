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

export function getColorNames() {
    return {
        '#000000': 'Black',
        '#000080': 'Navy',
        '#00008B': 'DarkBlue',
        '#0000CD': 'MediumBlue',
        '#0000FF': 'Blue',
        '#006400': 'DarkGreen',
        '#008000': 'Green',
        '#008080': 'Teal',
        '#008B8B': 'DarkCyan',
        '#00BFFF': 'DeepSkyBlue',
        '#00CED1': 'DarkTurquoise',
        '#00FA9A': 'MediumSpringGreen',
        '#00FF7F': 'SpringGreen',
        '#00FFFF': 'Aqua',
        '#00FFFF': 'Cyan',
        '#191970': 'MidnightBlue',
        '#1E90FF': 'DodgerBlue',
        '#20B2AA': 'LightSeaGreen',
        '#228B22': 'ForestGreen',
        '#2E8B57': 'SeaGreen',
        '#2F4F4F': 'DarkSlateGray',
        '#32CD32': 'LimeGreen',
        '#3CB371': 'MediumSeaGreen',
        '#40E0D0': 'Turquoise',
        '#4169E1': 'RoyalBlue',
        '#4682B4': 'SteelBlue',
        '#483D8B': 'DarkSlateBlue',
        '#48D1CC': 'MediumTurquoise',
        '#4B0082': 'Indigo',
        '#556B2F': 'DarkOliveGreen',
        '#5F9EA0': 'CadetBlue',
        '#6495ED': 'CornflowerBlue',
        '#66CDAA': 'MediumAquaMarine',
        '#696969': 'DimGray',
        '#6A5ACD': 'SlateBlue',
        '#6B8E23': 'OliveDrab',
        '#708090': 'SlateGray',
        '#778899': 'LightSlateGray',
        '#7B68EE': 'MediumSlateBlue',
        '#7CFC00': 'LawnGreen',
        '#7FFF00': 'Chartreuse',
        '#7FFFD4': 'Aquamarine',
        '#800000': 'Maroon',
        '#800080': 'Purple',
        '#808000': 'Olive',
        '#808080': 'Gray',
        '#87CEEB': 'SkyBlue',
        '#87CEFA': 'LightSkyBlue',
        '#8A2BE2': 'BlueViolet',
        '#8B0000': 'DarkRed',
        '#8B008B': 'DarkMagenta',
        '#8B4513': 'SaddleBrown',
        '#8FBC8F': 'DarkSeaGreen',
        '#90EE90': 'LightGreen',
        '#9370D8': 'MediumPurple',
        '#9400D3': 'DarkViolet',
        '#98FB98': 'PaleGreen',
        '#9932CC': 'DarkOrchid',
        '#9ACD32': 'YellowGreen',
        '#A0522D': 'Sienna',
        '#A52A2A': 'Brown',
        '#A9A9A9': 'DarkGray',
        '#ADD8E6': 'LightBlue',
        '#ADFF2F': 'GreenYellow',
        '#AFEEEE': 'PaleTurquoise',
        '#B0C4DE': 'LightSteelBlue',
        '#B0E0E6': 'PowderBlue',
        '#B22222': 'FireBrick',
        '#B8860B': 'DarkGoldenRod',
        '#BA55D3': 'MediumOrchid',
        '#BC8F8F': 'RosyBrown',
        '#BDB76B': 'DarkKhaki',
        '#C0C0C0': 'Silver',
        '#C71585': 'MediumVioletRed',
        '#CD5C5C': 'IndianRed',
        '#CD853F': 'Peru',
        '#D2691E': 'Chocolate',
        '#D2B48C': 'Tan',
        '#D3D3D3': 'LightGrey',
        '#D87093': 'PaleVioletRed',
        '#D8BFD8': 'Thistle',
        '#DA70D6': 'Orchid',
        '#DAA520': 'GoldenRod',
        '#DC143C': 'Crimson',
        '#DCDCDC': 'Gainsboro',
        '#DDA0DD': 'Plum',
        '#DEB887': 'BurlyWood',
        '#E0FFFF': 'LightCyan',
        '#E6E6FA': 'Lavender',
        '#E9967A': 'DarkSalmon',
        '#EE82EE': 'Violet',
        '#EEE8AA': 'PaleGoldenRod',
        '#F08080': 'LightCoral',
        '#F0E68C': 'Khaki',
        '#F0F8FF': 'AliceBlue',
        '#F0FFF0': 'HoneyDew',
        '#F0FFFF': 'Azure',
        '#F4A460': 'SandyBrown',
        '#F5DEB3': 'Wheat',
        '#F5F5DC': 'Beige',
        '#F5F5F5': 'WhiteSmoke',
        '#F5FFFA': 'MintCream',
        '#F8F8FF': 'GhostWhite',
        '#FA8072': 'Salmon',
        '#FAEBD7': 'AntiqueWhite',
        '#FAF0E6': 'Linen',
        '#FAFAD2': 'LightGoldenRodYellow',
        '#FDF5E6': 'OldLace',
        '#FF0000': 'Red',
        '#FF00FF': 'Fuchsia',
        '#FF00FF': 'Magenta',
        '#FF1493': 'DeepPink',
        '#FF4500': 'OrangeRed',
        '#FF6347': 'Tomato',
        '#FF69B4': 'HotPink',
        '#FF7F50': 'Coral',
        '#FF8C00': 'DarkOrange',
        '#FFA07A': 'LightSalmon',
        '#FFA500': 'Orange',
        '#FFB6C1': 'LightPink',
        '#FFC0CB': 'Pink',
        '#FFD700': 'Gold',
        '#FFDAB9': 'PeachPuff',
        '#FFDEAD': 'NavajoWhite',
        '#FFE4B5': 'Moccasin',
        '#FFE4C4': 'Bisque',
        '#FFE4E1': 'MistyRose',
        '#FFEBCD': 'BlanchedAlmond',
        '#FFEFD5': 'PapayaWhip',
        '#FFF0F5': 'LavenderBlush',
        '#FFF5EE': 'SeaShell',
        '#FFF8DC': 'Cornsilk',
        '#FFFACD': 'LemonChiffon',
        '#FFFAF0': 'FloralWhite',
        '#FFFAFA': 'Snow',
        '#FFFF00': 'Yellow',
        '#FFFFE0': 'LightYellow',
        '#FFFFF0': 'Ivory',
        '#FFFFFF': 'White',
    }
}

export function getName(colorHex) {
    const colorNames = getColorNames();
    const colorUpperCase = colorHex.toUpperCase();
    return colorUpperCase in colorNames ? colorNames[colorUpperCase] : '';
}

export function getSchemeTypes() {
    return [
        'complementary',
        'splitComplementary',
        'splitComplementaryCW',
        'splitComplementaryCCW',
        'triadic',
        'clash',
        'tetradic',
        'fourToneCW',
        'fourToneCCW',
        'fiveToneA',
        'fiveToneB',
        'fiveToneC',
        'fiveToneD',
        'fiveToneE',
        'sixToneCW',
        'sixToneCCW',
        'neutral',
        'analogous',
    ]
}

export function generateScheme(colorHex, schemeType) {
    var color = Color(colorHex);
    const test = color.tetradicScheme();
    console.log(test);
}