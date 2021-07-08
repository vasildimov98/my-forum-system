let mybutton = document.getElementById("btn-back-to-top");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    if (
        document.body.scrollTop > 20 ||
        document.documentElement.scrollTop > 20
    ) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}
// When the user clicks on the button, scroll to the top of the document
mybutton.addEventListener("click", backToTop);

function backToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

const userInfoDivs = document
    .querySelectorAll(".user-info-wrapper");

userInfoDivs
    .forEach(x => x
        .addEventListener("click", hideOrShowComment));

function hideOrShowComment() {
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

function textCounter(field, field2, maxlimit) {
    var countfield = document.getElementById(field2);
    if (field.value.length > maxlimit) {
        field.value = field.value.substring(0, maxlimit);
        return false;
    } else {
        countfield.innerHTML = field.value.length + "/300"
    }
}

function chageCommentBox(commentBox, parentId) {
    commentBox.querySelector("input[name='ParentId']").value = parentId;

    console.log(commentBox.querySelector("input[name='ParentId']"));

    var commentBoxDisplay = commentBox.style.display;

    if (commentBoxDisplay == 'none') {
        commentBox.style.display = 'block';
        return;
    }

    commentBox.style.display = 'none';
}
