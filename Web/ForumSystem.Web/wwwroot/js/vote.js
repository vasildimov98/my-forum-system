function sendVote(id, isUpVote, url, votesCountDiv) {
    let json = JSON.stringify({ id, isUpVote });

    let token = document.getElementsByName("__RequestVerificationToken")[0].value;

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
            let votesCount = data.votesCount;

            votesCountDiv.textContent = votesCount;
        });
}