document.addEventListener("DOMContentLoaded", ready);
function ready(){
    stars();
    heart();
}

function stars(){
    let stars = document.getElementById("starsRatingBlock");
    stars.addEventListener('mousemove', (event) => {
        var rect = event.target.getBoundingClientRect();
        var x = event.clientX - rect.left;  
        let procentX=0;
        if (x>100) procentX=100;
        else if (x>75) procentX=80;
        else if (x>50) procentX=60;
        else if (x>25) procentX=40;
        else if (x>0) procentX=20;  

        var style = stars.style;
        style.setProperty('--starsRatingBlockWidth', procentX+'%');
    });
    stars.addEventListener('mouseleave', (event) => {
        var style = stars.style;
        style.setProperty('--starsRatingBlockWidth', '0%');
    });

    stars.addEventListener('mouseup', (event) => {
        var style = stars.style;
        style.setProperty('--starsRatingBG', "#716B6B");
    });
    stars.addEventListener('mousedown', (event) => {
        var style = stars.style;
        style.setProperty('--starsRatingBG', "#8BC65D");
    });
}


function heart(){
    let heart = document.getElementById("heartBlock"); 

    heart.addEventListener('mouseup', (event) => {
        let style = heart.style;
        style.setProperty('--heartBlockBeforeBG', "#DA1A1A");
        style.setProperty('--heartBlockAfterBG', "#ffffff");
        if (!heart.classList.contains("heartBlockActive"))
            heart.classList.add("heartBlockActive");
        else
            heart.classList.remove("heartBlockActive");

    });
    heart.addEventListener('mousedown', (event) => {
        let style = heart.style;
        style.setProperty('--heartBlockBeforeBG', "#FFA740");
        style.setProperty('--heartBlockAfterBG', "#FFA740");
    });
}
