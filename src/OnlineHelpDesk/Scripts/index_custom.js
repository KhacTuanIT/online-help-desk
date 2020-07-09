window.onscroll = function () { myFunction() };

var navbar = document.getElementById("header");
var sticky = navbar.offsetTop;
var footer = document.getElementById("footer");
var stickybot = footer.offsetTop + footer.offsetHeight;

function myFunction() {
	if (window.pageYOffset >= sticky) {
		navbar.classList.add("sticky");
	} else {
		navbar.classList.remove("sticky");
	}
}
$(window).bind("load", function () {

	var footerHeight = 0,
		footerTop = 0,
		$footer = $("#footer");

	positionFooter();

	function positionFooter() {

		footerHeight = $footer.height();
		footerTop = ($(window).scrollTop() + $(window).height() - footerHeight) + "px";

		if (($(document.body).height() + footerHeight) < $(window).height()) {
			$footer.css({
				position: "absolute",
				bottom: 0
			})
		} else {
			$footer.css({
				position: "static"
			})
		}

	}

	$(window)
		.scroll(positionFooter)
		.resize(positionFooter)

});