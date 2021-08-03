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
