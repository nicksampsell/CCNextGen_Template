var clickable = document.querySelectorAll('tr[data-url]');

clickable.forEach(x => {
    x.addEventListener('click', (event) => {
        if (event.target.tagName !== 'BUTTON' && event.target.tagName !== 'INPUT' && event.target.tagName !== 'A') {
            let url = x.dataset.url;
            window.location.replace(url);
        }
    })
})