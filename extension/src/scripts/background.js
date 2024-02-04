
chrome.runtime.onMessage.addListener((request, sender, clbk) => {
  if (request.method === "GET") {
    
    var url = request.url;

    fetch(url)
      .then(response => response.text())
      .then(response => clbk(response))
      .catch(e => console.log(e))
    
    return true;
  }

  if (request.method === 'PUT') {
    
    fetch(request.url, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: request.data
    })
      .then(response => response.json())
      .then(response => clbk(response))
      .catch(error => console.log('Error:', error));

    return true;
  }
})