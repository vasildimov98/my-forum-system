var textAreas = document
    .getElementsByClassName("prevent-line-break");

for (let textArea of textAreas) {
    textArea
        .addEventListener("onkeydown", function (event) {
            var key = event.keyCode;
            if (key == 13 && !event.shiftKey)
                return false;
        });
}