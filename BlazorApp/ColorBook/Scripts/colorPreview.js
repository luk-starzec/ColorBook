window.colorPreview = {

    getWindowWidth: () => window.innerWidth,

    getWindowHeight: () => window.innerHeight,

    getElementXY: (elementId) => {
        var rect = document.getElementById(elementId).getBoundingClientRect();
        var top = Math.round(rect.top + window.scrollY);
        var left = Math.round(rect.left + window.scrollX);
        return [left, top];
    },

    drawImg: (canvasId, data) => {
        const ctx = document.getElementById(canvasId).getContext('2d');
        const img = new Image();
        img.src = data;
        ctx.drawImage(img, 0, 0);
    },

    getRGBA: (canvasId) => {
        const canvas = document.getElementById(canvasId);
        const width = canvas.width;
        const height = canvas.height;
        const x = width / 2;
        const y = height / 2;

        const ctx = canvas.getContext('2d');
        const pixel = ctx.getImageData(x, y, 1, 1).data

        const array = Array.from(pixel);
        return array;
    },

    initPreview: (canvasId, scale) => {
        const canvas = document.getElementById(canvasId);
        canvas.width = canvas.offsetWidth;
        canvas.height = canvas.offsetHeight;

        const ctx = canvas.getContext('2d');
        ctx.scale(scale, scale);
    },

    setPreview: (imageId, canvasId, x, y, scale) => {
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
    },

    componentToHex: (c) => {
        var hex = c.toString(16);
        return hex.length == 1 ? "0" + hex : hex;
    },

    rgbToHex: (r, g, b) => {
        return "#" + colorPreview.componentToHex(r) + colorPreview.componentToHex(g) + colorPreview.componentToHex(b);
    },
}