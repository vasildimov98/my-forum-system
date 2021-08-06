function showTextAreaForEditingComment(commentContentSection, commentId) {
    const uniqueTinyMceTextAreaId = `tinyMce${commentId}`;

    let commentSection = commentContentSection
        .querySelector(".comment-section");

    var currContentAsHtml;

    if (commentSection) {
        currContentAsHtml = commentSection
            .innerHTML;
    }

    if (commentSection) {
        let textArea = document.createElement("textarea");

        textArea.setAttribute("id", uniqueTinyMceTextAreaId);

        commentContentSection.removeChild(commentSection);

        commentContentSection.append(textArea);

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

        tinymce
            .get(uniqueTinyMceTextAreaId)
            .setContent(currContentAsHtml);

        return;
    }

    Swal.fire({
        title: 'Do you want to save the changes?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: `Save`,
        denyButtonText: `Don't save`,
    }).then(async (result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            Swal.fire('Saved!', '', 'success');
            saveEditComment(uniqueTinyMceTextAreaId, commentId, commentContentSection);
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info');
            let contentAsHtml = await getCurrCommentContent(commentId);
            let content = createElementFromHTML(contentAsHtml);
            showCommentOutsideTextArea(content, commentContentSection);
        }
    })
}

async function getCurrCommentContent(commentId) {
    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    return await fetch(`/api/comments?commentId=${commentId}`, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
    }).then(res => res.text());
}

async function saveEditComment(uniqueTinyMceTextAreaId, commentId, commentContentSection) {
    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    let editContent = tinymce
        .get(uniqueTinyMceTextAreaId)
        .getContent();

    let json = JSON.stringify({
        commentId,
        editContent,
    });

    let jsonResponse = await fetch("/api/comments/edit", {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
        body: json
    }).then(res => res.json());

    let content = createElementFromHTML(jsonResponse["sanitizeContent"]);
    showCommentOutsideTextArea(content, commentContentSection);
}

function showCommentOutsideTextArea(content, commentContentSection) {
    let commentSection = document.createElement("div");
    commentSection.classList.add("comment-section");
    commentSection.append(content);

    let wrap = document.createElement("div");
    wrap.appendChild(commentSection.cloneNode(true));

    commentContentSection.innerHTML = wrap.innerHTML;
}

function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();

    // Change this to div.childNodes to support multiple top-level nodes
    return div.firstChild;
}