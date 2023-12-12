function createBlob(data, type) {
    return new Blob([data], { type: type });
}

/**
    * gives a download for the blob
    * @param blob blob to provide a download for
    * @param fileName file name for the blob download
    */
function blobToFile(blob, fileName) {
  if (navigator.msSaveOrOpenBlob) {
    return navigator.msSaveOrOpenBlob(blob, fileName);
    }

  var a = document.createElement("a");
  document.body.appendChild(a);
  a.style.display = "none";
  var url = URL.createObjectURL(blob);
  a.href = url;
  a.download = fileName;
  a.click();
  a.remove();
}

function saveDataToFile(data, type, fileName) {
    var blob = createBlob(data, type);
    blobToFile(blob, fileName);
}