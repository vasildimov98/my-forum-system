let mybutton = document.getElementById("btn-back-to-top");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
    scrollFunction();
};

const allCommentsDivElements = document
    .getElementsByClassName("container-fluid");

console.log(allCommentsDivElements);

for (var i = 0; i < allCommentsDivElements.length; i++) {
    const postCommentSection = allCommentsDivElements[i];

    postCommentSection
        .addEventListener("click", commentOnPost);
}

postCommentSection
    .addEventListener("click", commentOnPost);

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
    commentBox.querySelector("input[name='parentId']").value = parentId;

    var commentBoxDisplay = commentBox.style.display;

    if (commentBoxDisplay == 'none') {
        commentBox.style.display = 'block';
        return;
    }

    commentBox.style.display = 'none';
}

async function commentOnPost(event) {
    var targetElement = event.target;

    console.log(event.currentTarget);
    console.log(event.target);

    if (!targetElement
        || !targetElement.className.includes("my-post-comment-btn"))
        return;

    const formNode = targetElement
        .parentNode
        .parentNode;

    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    const postId = formNode.querySelector("input[name=postId]").value;
    const parentId = formNode.querySelector("input[name=parentId]").value;
    const content = tinyMCE.activeEditor.getContent();

    const json = JSON.stringify({
        postId,
        parentId,
        content,
    });

    var jsonResponse = await fetch("/api/comments", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
        body: json
    }).then(res => res.json());

    const id = jsonResponse["id"];
    const imageSrc = jsonResponse["imageSrc"];
    const username = jsonResponse["userUserName"];
    const sanitizeContent = jsonResponse["sanitizeContent"];
    const voteTypeCount = jsonResponse["voteTypeCount"];
    const votesCountId = jsonResponse["votesCountId"];
    const formCommentId = jsonResponse["formCommentId"];

    let commentDivSection = `<div class="container-fluid">
                <div class="row">
                    <div class="col-md-12 p-0">
                        <div class="card mb-3">
                            <div class="card-header">
                                <div class="user-info-wrapper media flex-wrap w-100 align-items-center">
                                    <img src=${imageSrc} width="20" class="d-sm-block ui-w-40 rounded-circle">

                                    <div class="media-body ml-3">
                                        <h6>${username}</h6>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                ${sanitizeContent}
                            </div>
                            <div class="card-footer">
                                <div class="container">
                                    <div class="row">
                                        <div class="col-md-12 flex-row">
                                            <a class="btn mb-2" href="javascript:;" onclick="sendVote(${id}, true, '/api/votes/comment')">
                                                <i class="fa fa-arrow-up"></i>
                                            </a>
                                            <div class="d-inline mb-lg-5" id=${votesCountId}>${voteTypeCount}</div>
                                            <a class="btn mb-2" href="javascript:;" onclick="sendVote(${id}, false, '/api/votes/comment')">
                                                <i class="fa fa-arrow-down"></i>
                                            </a>
                                            <button class="btn mb-2" onclick="chageCommentBox(${formCommentId}, ${id})">
                                                <i class="fa fa-comment-dots"></i>
                                                Reply
                                            </button>
                                        </div>
                                    </div>

                                    <div class="row m-0">
                                        <div class="col-md-12 m-0">
                                            <form id=${formCommentId} asp-controller="Comments" asp-action="Create" method="post" style="display: none">
                                                <partial name="_AddCommentInputs" model="new CommentInputModel() { PostId = comment.PostId }" />
                                            </form>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`;
}
