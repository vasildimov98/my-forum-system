$('#myPostCommentBtn').click(function () {
    tinyMCE.triggerSave();
});

$.validator.setDefaults({ ignore: "" });

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
