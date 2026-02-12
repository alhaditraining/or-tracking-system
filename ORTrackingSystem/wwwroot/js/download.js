window.downloadFile = function (fileName, base64String, contentType) {
    const linkSource = `data:${contentType};base64,${base64String}`;
    const downloadLink = document.createElement('a');
    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
};
