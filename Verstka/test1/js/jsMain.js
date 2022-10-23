window.addEventListener("load", function (event) {
    let navItems = document.getElementsByClassName('navItem');
    for (let i = 0; i < navItems.length; i++) {       
        navItems[i].onclick = function () {
            console.log( navItems[i],"n");
            clearCurrent(navItems);
            navItems[i].classList.add('current');
        };
    }
});

function clearCurrent(navItems) {    
    for (var i = 0; i < navItems.length; i++) {
        navItems[i].classList.remove('current');
    };

}
