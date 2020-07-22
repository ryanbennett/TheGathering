function showHide(id) {
    var checkBox = document.getElementById("check" + id);
    var text = document.getElementById("text" + id);
    if (checkBox.checked == true) {
        text.style.display = "block";
    }
    else {
        text.style.display = "none";
    }
}

function loadAll() {
    for (var i = 1; i <= 3; i++) {
        showHide(i);
    }
}