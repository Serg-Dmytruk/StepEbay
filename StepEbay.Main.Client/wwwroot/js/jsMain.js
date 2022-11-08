window.addEventListener("load", function (event) {
    let navItems = document.getElementsByClassName('navItem');
    for (let i = 0; i < navItems.length; i++) {
        navItems[i].onclick = function () {
            console.log(navItems[i], "n");
            clearCurrent(navItems);
            navItems[i].classList.add('current');
        };
    }

     //this.setInterval(() => {

     //    if (window.matchMedia("(min-width: 576px)").matches) {
     //        /* the viewport is at least 400 pixels wide */
     //        console.log(" least 576 ");
     //        document.getElementById("menubg1").style.display = "block";
     //        document.getElementById("menubg2").style.display = "none";
     //    } else {
     //        console.log(" least than 576 ");
     //        document.getElementById("menubg1").style.display = "none";
     //        document.getElementById("menubg2").style.display = "block";
     //        /* the viewport is less than 400 pixels wide */

     //    }
     //}, 1000);

    this.setInterval(() => {

        let picture = document.getElementById('mainPicture');
        let pictures = document.getElementById('pictures');
        if (picture) {
            picture.style.height = `${picture.offsetWidth}px`;
            pictures.style.height = `${picture.offsetWidth}px`;
        }
        
    }, 1000);

});

function clearCurrent(navItems) {
    for (var i = 0; i < navItems.length; i++) {
        navItems[i].classList.remove('current');
    };

}

function dropDownMenu() {
    let dropdown = document.getElementById('drop-down-menu');
    dropdown.style.top = dropdown.style.top < '0' ? '0' : '-300px';
}

