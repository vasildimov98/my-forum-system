const allCommentsDivElements = document
    .getElementsByClassName("container-fluid");

const commentOnPostSection = document
    .getElementsByClassName("comment-on-post")[0];

commentOnPostSection
    .addEventListener("click", commentOnCurrSection)

for (var i = 0; i < allCommentsDivElements.length; i++) {
    var currComment = allCommentsDivElements[i];

    currComment
        .addEventListener("click", commentOnCurrSection)
}

async function commentOnCurrSection(event) {
    var targetElement = event.target;
    var currentTarget = event.currentTarget;

    if (!targetElement
        || !targetElement.className.includes("my-post-comment-btn"))
        return;

    const formNode = currentTarget
        .querySelector(".comment-form");

    const commentsSection = currentTarget
        .querySelector(".comments-section");

    tinyMCE.triggerSave();

    const postId = formNode.querySelector("input[name=postId]").value;
    const parentId = formNode.querySelector("input[name=parentId]").value;
    const content = formNode.querySelector("textarea[name=content]").value;
    const clearContent = clearContentOfHtmlTags(content);

    if (!clearContent.replace(" ", "")) {
        const alertDiv = formNode
            .querySelector(".alert");

        alertDiv.style.display = "block";

        setTimeout(function () {
            alertDiv.style.display = "none";
        }, 3000);

        event.stopPropagation();
        return;
    }

    event.stopPropagation();

    const json = JSON.stringify({
        postId,
        parentId,
        content,
    });

    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    var jsonResponse = await fetch("/api/comments", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
        body: json
    }).then(res => res.json());

    let newCommentNode = getCommentDivSectionFromResponse(jsonResponse);

    commentsSection.insertBefore(newCommentNode, commentsSection.firstChild);

    tinyMCE.activeEditor.setContent('');
    formNode.querySelector("textarea[name=content]").value = "";

    if (!currentTarget.className.includes("comment-on-post")) {
        formNode.style.display = 'none';
    }

    newCommentNode
        .addEventListener("click", commentOnCurrSection);

    let deleteBtn = newCommentNode
        .querySelector(".delete-btn");

    deleteBtn
        .addEventListener("click", showSweetAlertDialog);

    tinymce.init({
        selector: "textarea",
        resize: false,
        plugins: [
            "image paste table link code media"
        ],
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    const userInfoDivs = document
        .querySelectorAll(".user-info-wrapper");

    userInfoDivs
        .forEach(x => x
            .addEventListener("click", toggleComments));
}

function getCommentDivSectionFromResponse(jsonResponse) {
    const id = jsonResponse["id"];
    const postId = jsonResponse["postId"];
    const imageSrc = jsonResponse["imageSrc"];
    const username = jsonResponse["userUserName"];
    const sanitizeContent = jsonResponse["sanitizeContent"];
    const voteTypeCount = jsonResponse["voteTypeCount"];
    const votesCountId = jsonResponse["votesCountId"];
    const formCommentId = jsonResponse["formCommentId"];
    const commentContentSectionId = jsonResponse["commentContentSectionId"];

    let commentAsHtmlString = `<div class="container-fluid">
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
                        <div id="${commentContentSectionId}" class="card-body">
                                <div class="comment-section">
                                    ${sanitizeContent}
                                </div>
                            </div>
                        <div class="card-footer">
                            <div>
                                <div class="row">
                                    <div class="col-md-12 flex-row">
                                        <a class="btn mb-2" href="javascript:;" onclick="sendVote(${id}, true, '/api/votes/comment')">
                                            <i class="fa fa-arrow-up"></i>
                                        </a>
                                        <div class="d-inline mb-lg-5" id=${votesCountId}>${voteTypeCount}</div>
                                        <a class="btn mb-2" href="javascript:;" onclick="sendVote(${id}, false, '/api/votes/comment')">
                                            <i class="fa fa-arrow-down"></i>
                                        </a>
                                        <button class="btn mb-2" onclick="toggleCommentBoxEditor(${formCommentId}, ${id})">
                                            <i class="fa fa-comment-dots"></i>
                                            Reply
                                        </button>

                                        <button class="btn mb-2" onclick="showTextAreaForEditingComment(${commentContentSectionId}, ${id})">
                                            <i class="far fa-edit"></i>
                                            Edit
                                        </button>

                                        <input type="hidden" name="commentId" value="${id}"/>
                                        <button class="delete-btn btn mb-2" onclick="toggleCommentBoxEditor(@comment.FormCommentId, @comment.Id)">
                                            <i class="fa fa-trash alert-danger"></i>
                                            Delete
                                        </button>
                                    </div>
                                </div>

                                <div class="row m-0">
                                    <div class="col-md-12 m-0">
                                        <form id=${formCommentId} class="comment-form" asp-controller="Comments" asp-action="Create" method="post" style="display: none">
                                            <input type="hidden" name="postId" value="${postId}" />
                                            <input type="hidden" name="parentId" value="0" />
                                            <div class="form-group">
                                                <textarea asp-for="Content" name="content" class="form-control"></textarea>
                                                <span asp-validation-for="Content" class="text-danger"></span>
                                            </div>

                                            <div class="alert alert-danger" style="display: none" role="alert">
                                                Content is required!
                                            </div>

                                            <div class="float-right">
                                                <input type="button" class="btn btn-primary mb-3 my-post-comment-btn my-post-btn" value="Comment" />
                                            </div>
                                        </form>
                                    </div>

                                </div>
                                <div class="row">
                                        <div class="col-md-12 comments-section">
                                        </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;

    var div = document.createElement("div");

    div.innerHTML = commentAsHtmlString;

    return div.firstChild;
}

function clearContentOfHtmlTags(html) {
    var div = document.createElement("div");
    div.innerHTML = html;
    var clearContent = div.textContent || div.innerText || "";

    return clearContent;
}
