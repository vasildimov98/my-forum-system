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
        countfield.innerHTML = field.value.length + "/100"
    }
}

function sendVote(postId, isUpVote) {
    var json = JSON.stringify({ postId, isUpVote });
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    fetch("/api/votes",
        {
            method: "Post",
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
            },
            body: json,
        })
        .then(res => res.json())
        .then(data => {
            var votesCount = data.votesCount;
            var votesCountDiv = document.getElementById("votesCount");

            votesCountDiv.textContent = votesCount;
        });
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
