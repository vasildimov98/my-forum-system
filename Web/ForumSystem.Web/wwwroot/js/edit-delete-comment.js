function showTextAreaForEditingComment(commentContentSection, commentId) {
    let firstChild = commentContentSection.querySelector("div");
    let contentAsHtml = firstChild.innerHTML;

    if (commentContentSection.tagName == "DIV") {
        let textArea = document.createElement("textarea");

        textArea.classList.add("form-control");

        commentContentSection.removeChild(firstChild);

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

        tinymce.activeEditor.setContent(contentAsHtml);

        return;
    }
}