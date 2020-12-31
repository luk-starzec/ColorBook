
drawImg = (canvasId, data) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    const img = new Image();
    img.src = data;
    ctx.drawImage(img, 0, 0);
}

getRGBA = (canvasId) => {
    const canvas = document.getElementById(canvasId);
    const width = canvas.width;
    const height = canvas.height;
    const x = width / 2;
    const y = height / 2;

    const ctx = canvas.getContext('2d');
    const pixel = ctx.getImageData(x, y, 1, 1).data
    console.log(pixel);

    const array = Array.from(pixel);
    console.log(array);

    return array;
}

initPreview = (canvasId, scale) => {
    const canvas = document.getElementById(canvasId);
    const width = canvas.width;
    const height = canvas.height;

    const ctx = canvas.getContext('2d');
    ctx.scale(scale, scale);
}

setPreview = (imageId, canvasId, x, y, scale) => {
    const image = document.getElementById(imageId);

    const canvas = document.getElementById(canvasId);
    const width = canvas.width / scale;
    const height = canvas.height / scale;

    const x1 = x - width / 2;
    const y1 = y - height / 2;

    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, width, height);
    ctx.drawImage(image, x1, y1, width, height, 0, 0, width, height);

    const px = width / 2 - 1;
    const py = height / 2 - 1;
    ctx.lineWidth = 1 / scale;
    ctx.strokeStyle = "white";
    ctx.strokeRect(px, py, 2, 2);


    //const ctxImage = document.getElementById(canvasId).getContext('2d');
    //const previewData = ctxImage.getImageData(x1, y1, width, height);

    //const ctxPreview = document.getElementById(previewId).getContext('2d');
    ////ctxPreview.putImageData(previewData, 0, 0, 0, 0, 50, 50);
    //ctxPreview.putImageData(previewData, 0, 0);
}

componentToHex = (c) => {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

rgbToHex = (r, g, b) => {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}


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