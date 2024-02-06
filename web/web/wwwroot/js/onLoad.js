

window.js_onLoad = () => {
    // INITIALIZATION OF HEADER
    // =======================================================
    new HSHeader('#header').init()

    // INITIALIZATION OF MEGA MENU
    // =======================================================
    new HSMegaMenu('.js-mega-menu', {
        desktop: {
            position: 'left'
        }
    })

    // INITIALIZATION OF SHOW ANIMATIONS
    // =======================================================
    new HSShowAnimation('.js-animation-link')

    // INITIALIZATION OF BOOTSTRAP VALIDATION
    // =======================================================
    HSBsValidation.init('.js-validate', {
        onSubmit: data => {
            data.event.preventDefault()
            alert('Submited')
        }
    })

    // INITIALIZATION OF GO TO
    // =======================================================
    new HSGoTo('.js-go-to')

    // INITIALIZATION OF SELECT
    // =======================================================
    HSCore.components.HSTomSelect.init('.js-select', {
        render: {
            'option': function (data, escape) {
                return data.optionTemplate
            },
            'item': function (data, escape) {
                return data.optionTemplate
            }
        }
    })

    // INITIALIZATION OF STICKY BLOCKS
    // =======================================================
    new HSStickyBlock('.js-sticky-block', {
        targetSelector: document.getElementById('header').classList.contains('navbar-fixed') ? '#header' : null
    })

    // INITIALIZATION OF popovers
    // =======================================================
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    })

    // INITIALIZATION OF BOOTSTRAP DROPDOWN
    // =======================================================
    HSBsDropdown.init()

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })


   

}



//window.js_onLoadNouislider = () => {
//    // INITIALIZATION OF NOUISLIDER
//    // =======================================================
//    HSCore.components.HSNoUISlider.init('.js-nouislider')
//}

window.js_onLoadJsNavScroller = () => {
    // INITIALIZATION OF NAV SCROLLER
    // =======================================================
    new HsNavScroller('.js-nav-scroller')
}