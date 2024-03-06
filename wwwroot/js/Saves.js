function toggleUnsave(i) {
    let checkbox = document.getElementById('cb-' + i.toString()); //cb-[#i]
    let img = document.getElementById('img-' + i.toString()); //img-[#i]

    if (checkbox.checked) {
        img.src = "/assets/icons/heart.png";
    } else {
        img.src = "/assets/icons/love.png";
    }
}