function sendVote(id, isUpVote, url) {
    var json = JSON.stringify({ id, isUpVote });
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    fetch(url,
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
            var votesCountDiv = document.getElementById('votesCount' + id);

            votesCountDiv.textContent = votesCount;
        });
}