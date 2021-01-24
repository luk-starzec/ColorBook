'use strict';

window.colorPreview = {

    drawImg: drawImg = function (canvasId, data) {
        var ctx = document.getElementById(canvasId).getContext('2d');
        var img = new Image();
        img.src = data;
        ctx.drawImage(img, 0, 0);
    },

    getRGBA: getRGBA = function (canvasId) {
        var canvas = document.getElementById(canvasId);
        var width = canvas.width;
        var height = canvas.height;
        var x = width / 2;
        var y = height / 2;

        var ctx = canvas.getContext('2d');
        var pixel = ctx.getImageData(x, y, 1, 1).data;
        console.log(pixel);

        var array = Array.from(pixel);
        console.log(array);

        return array;
    },

    initPreview: initPreview = function (canvasId, scale) {
        var canvas = document.getElementById(canvasId);
        var width = canvas.width;
        var height = canvas.height;

        var ctx = canvas.getContext('2d');
        ctx.scale(scale, scale);
    },

    setPreview: setPreview = function (imageId, canvasId, x, y, scale) {
        var image = document.getElementById(imageId);

        var canvas = document.getElementById(canvasId);
        var width = canvas.width / scale;
        var height = canvas.height / scale;

        var x1 = x - width / 2;
        var y1 = y - height / 2;

        var ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, width, height);
        ctx.drawImage(image, x1, y1, width, height, 0, 0, width, height);

        var px = width / 2 - 1;
        var py = height / 2 - 1;
        ctx.lineWidth = 1 / scale;
        ctx.strokeStyle = "white";
        ctx.strokeRect(px, py, 2, 2);

        //const ctxImage = document.getElementById(canvasId).getContext('2d');
        //const previewData = ctxImage.getImageData(x1, y1, width, height);

        //const ctxPreview = document.getElementById(previewId).getContext('2d');
        ////ctxPreview.putImageData(previewData, 0, 0, 0, 0, 50, 50);
        //ctxPreview.putImageData(previewData, 0, 0);
    },

    componentToHex: componentToHex = function (c) {
        var hex = c.toString(16);
        return hex.length == 1 ? "0" + hex : hex;
    },

    rgbToHex: rgbToHex = function (r, g, b) {
        return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
    }

};
//draw = () => {
//    var ctx = document.getElementById('canvas').getContext('2d');
//    var img = new Image();
//    img.onload = function () {
//        ctx.drawImage(img, 0, 0);
//        ctx.beginPath();
//        ctx.moveTo(30, 96);
//        ctx.lineTo(70, 66);
//        ctx.lineTo(103, 76);
//        ctx.lineTo(170, 15);
//        ctx.stroke();
//    };
//    img.src = 'https://mdn.mozillademos.org/files/5395/backdrop.png';
//    return true;
//}

