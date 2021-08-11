tinymce.init({
    selector: ".mytinymcetextarea",
    plugins: [
        "image paste table link code media autoresize"
    ],
    resize: false,
    setup: function (editor) {
        editor.on('change', function () {
            editor.save();
        });
    }
});
