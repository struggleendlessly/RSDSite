/*
* Chart.js wrapper
* @version: 3.0.0 (Mon, 25 Nov 2021)
* @requires: Chart.js v2.8.0
* @author: HtmlStream
* @event-namespace: .HSCore.components.HSValidation
* @license: Htmlstream Libraries (https://htmlstream.com/licenses)
* Copyright 2021 Htmlstream
*/

function isObject(item) {
	return (item && typeof item === 'object' && !Array.isArray(item));
}

function mergeDeep(target, ...sources) {
	if (!sources.length) return target;
	const source = sources.shift();

	if (isObject(target) && isObject(source)) {
		for (const key in source) {
			if (isObject(source[key])) {
				if (!target[key]) Object.assign(target, { [key]: {} });
				mergeDeep(target[key], source[key]);
			} else {
				Object.assign(target, { [key]: source[key] });
			}
		}
	}

	return mergeDeep(target, ...sources);
}

HSCore.components.HSChartJS = {
	init: function (el, options) {
		this.$el = typeof el === "string" ? document.querySelector(el) : el
		if (!this.$el) return

		this.defaults = {
			options: {
				responsive: true,
				maintainAspectRatio: false,
				plugins: {
					legend: {
						display: false
					},
					tooltip: {
						enabled: false,
						mode: 'nearest',
						prefix: '',
						postfix: '',
						hasIndicator: false,
						indicatorWidth: '8px',
						indicatorHeight: '8px',
						transition: '0.2s',
						lineWithLineColor: null,
						yearStamp: true
					},
				},
				gradientPosition: {
					x0: 0,
					y0: 0,
					x1: 0,
					y1: 0,
				}
			}
		}
		var dataSettings = this.$el.hasAttribute('data-hs-chartjs-options') ? JSON.parse(this.$el.getAttribute('data-hs-chartjs-options')) : {}

		this.settings = mergeDeep(this.defaults, {...options, ...dataSettings})

		/* Start : Init */

		var newChartJS = new Chart(el, this.settings);

		/* End : Init */

		return newChartJS;
	}
}
