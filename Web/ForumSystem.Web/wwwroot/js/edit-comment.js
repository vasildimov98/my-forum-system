async function showTextAreaForEditingComment(commentContentSection, commentId) {
    let commentSection = commentContentSection
        .querySelector(".comment-section");

    const uniqueTinyMceTextAreaId = `tinyMce${commentId}`;

    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    if (commentSection) {
        let textArea = document.createElement("textarea");
        let contentAsHtml = commentSection
            .innerHTML;

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
            .setContent(contentAsHtml);

        return;
    }

    let editContent = tinymce
        .get(uniqueTinyMceTextAreaId)
        .getContent();

    let json = JSON.stringify({
        commentId,
        editContent,
    });

    let textAreaWithContent = document
        .getElementById(uniqueTinyMceTextAreaId);

    let jsonResponse = await fetch("/api/comments/edit", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
        body: json
    }).then(res => res.json());

    commentContentSection.removeChild(textAreaWithContent);

    commentSection = document.createElement("div");
    commentSection.classList.add("comment-section");
    commentSection.append(createElementFromHTML(jsonResponse["sanitizeContent"]));

    let wrap = document.createElement("div");
    wrap.appendChild(commentSection.cloneNode(true));
    
    console.log(wrap.innerHTML);

    commentContentSection.innerHTML = wrap.innerHTML;
}

function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();

    // Change this to div.childNodes to support multiple top-level nodes
    return div.firstChild;
}