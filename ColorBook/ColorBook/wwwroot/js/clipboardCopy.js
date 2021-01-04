"use strict";

window.clipboardCopy = {
    copyText: function copyText(text) {
        return navigator.clipboard.writeText(text).then(function () {
            return true;
        })["catch"](function (error) {
            console.error(error);
            return false;
        });
    }
};

