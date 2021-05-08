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