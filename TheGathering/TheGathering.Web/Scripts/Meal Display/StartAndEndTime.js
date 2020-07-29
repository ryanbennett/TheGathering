function checkTime(id) {
    var start = document.getElementById("startTime" + id).value;
    var end = document.getElementById("endTime" + id).value;
    var formReturn = document.getElementById("formReturn" + id);
    var formSubmit = document.getElementById("submit");

    var startInt = parseInt(start.replaceAll(':', ''));
    var endInt = parseInt(end.replaceAll(':', ''));

    if (startInt >= endInt) {
        formReturn.innerText = "Start Time is later than End Time.";
        submit.disabled = true;
    }
    else {
        formReturn.innerText = "";
        submit.disabled = false;
    }

    return startInt < endInt;
}