function showTextAreaForEditingComment(commentContentSectionId, commentId) {
    const commentContentSection = document
        .getElementById(commentContentSectionId);

    let contentNode = commentContentSection.firstChild;
    let content = contentNode.innerHtml;

    console.log(content);

    if (contentNode.nodeType == "div") {
        let textArea = document.createElement("textarea");

        textArea.classList.add("form-control");
        textArea.innerHTML = content;

        commentContentSection.firstChild = textArea;

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
    }
}