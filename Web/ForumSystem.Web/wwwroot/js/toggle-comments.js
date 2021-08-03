const userInfoDivs = document
    .querySelectorAll(".user-info-wrapper");

userInfoDivs
    .forEach(x => x
        .addEventListener("click", toggleComments));

function toggleComments() {
    let cardBodyElement = this.parentNode.parentNode.childNodes[3];
    let cardFooterElement = this.parentNode.parentNode.childNodes[5];

    var displayOption = cardBodyElement.style.display;

    if (displayOption === "none") {
        cardBodyElement.style.display = "block";
        cardFooterElement.style.display = "block";
    } else {
        cardBodyElement.style.display = "none";
        cardFooterElement.style.display = "none";
    }
}

function toggleCommentBoxEditor(commentBox, parentId) {
    commentBox.querySelector("input[name='parentId']").value = parentId;

    var commentBoxDisplay = commentBox.style.display;

    if (commentBoxDisplay == 'none') {
        commentBox.style.display = 'block';
        return;
    }

    commentBox.style.display = 'none';
}

