const tx = document
    .getElementsByClassName("authoresize-area");
for (let i = 0; i < tx.length; i++) {
    tx[i]
        .setAttribute("style", "height:" + (tx[i].scrollHeight) + "px;overflow-y:hidden;");

    tx[i]
        .addEventListener("input", onInput, false);

    tx[i]
        .addEventListener("keypress", preventLineBreak, false);
}

function preventLineBreak(event) {
    var key = event.keyCode;
    if (key == 13 && !event.shiftKey)
        event.preventDefault();
}

function onInput() {
    this.style.height = "auto";
    this.style.height = (this.scrollHeight) + "px";
}