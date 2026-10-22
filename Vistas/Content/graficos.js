/* Dibuja con Chart.js cada <canvas data-grafico="{...}">. El servidor arma el JSON:
   {
     tipo: "bar" | "line" | "doughnut",
     etiquetas: [...],
     series: [{ nombre, valores, tipo?, formato?: "moneda" | "numero", ejeDerecho? }],
     horizontal?: bool
   }
   Así las páginas no tienen JavaScript propio. */
(function () {
    "use strict";

    var paleta = ["#0d6efd", "#20c997", "#fd7e14", "#6f42c1", "#dc3545", "#ffc107", "#0dcaf0", "#198754", "#d63384", "#6c757d"];
    var moneda = new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS", maximumFractionDigits: 0 });
    var numero = new Intl.NumberFormat("es-AR", { maximumFractionDigits: 1 });
    var compacto = new Intl.NumberFormat("es-AR", { notation: "compact", maximumFractionDigits: 1 });

    function formatear(valor, formato, corto) {
        if (formato === "moneda") {
            return corto ? "$ " + compacto.format(valor) : moneda.format(valor);
        }
        return numero.format(valor);
    }

    function transparente(color, alfa) {
        var n = parseInt(color.slice(1), 16);
        return "rgba(" + (n >> 16) + "," + ((n >> 8) & 255) + "," + (n & 255) + "," + alfa + ")";
    }

    function configurar(def) {
        var circular = def.tipo === "doughnut";
        var hayEjeDerecho = def.series.some(function (s) { return s.ejeDerecho; });

        var datasets = def.series.map(function (s, i) {
            var color = paleta[i % paleta.length];
            var tipo = s.tipo || def.tipo;
            return {
                type: tipo,
                label: s.nombre,
                data: s.valores,
                formato: s.formato,
                yAxisID: circular ? undefined : (s.ejeDerecho ? "y1" : "y"),
                xAxisID: circular ? undefined : "x",
                backgroundColor: circular ? paleta : (tipo === "line" ? transparente(color, 0.12) : transparente(color, 0.85)),
                borderColor: circular ? "#fff" : color,
                borderWidth: circular ? 2 : (tipo === "line" ? 2.5 : 0),
                borderRadius: tipo === "bar" ? 6 : 0,
                fill: tipo === "line" && !s.ejeDerecho,
                tension: 0.3,
                pointRadius: tipo === "line" && s.valores.length > 40 ? 0 : 3,
                order: tipo === "line" ? 0 : 1
            };
        });

        var formatoEje = def.series[0].formato;
        var formatoEjeDerecho = hayEjeDerecho ? def.series.filter(function (s) { return s.ejeDerecho; })[0].formato : null;
        var ejeValores = def.horizontal ? "x" : "y";
        var escalas = {};
        if (!circular) {
            escalas[ejeValores] = {
                beginAtZero: true,
                grid: { color: "rgba(0,0,0,.05)" },
                ticks: { callback: function (v) { return formatear(v, formatoEje, true); } }
            };
            escalas[def.horizontal ? "y" : "x"] = { grid: { display: false } };
            if (hayEjeDerecho) {
                escalas.y1 = {
                    position: "right",
                    beginAtZero: true,
                    grid: { display: false },
                    ticks: { precision: 0, callback: function (v) { return formatear(v, formatoEjeDerecho, true); } }
                };
            }
        }

        return {
            type: def.tipo,
            data: { labels: def.etiquetas, datasets: datasets },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: def.horizontal ? "y" : "x",
                interaction: { mode: circular ? "nearest" : "index", intersect: circular },
                cutout: circular ? "62%" : undefined,
                scales: escalas,
                plugins: {
                    legend: { display: circular || def.series.length > 1, position: "bottom", labels: { usePointStyle: true, boxWidth: 8 } },
                    tooltip: {
                        callbacks: {
                            label: function (ctx) {
                                var valor = def.horizontal ? ctx.parsed.x : (circular ? ctx.parsed : ctx.parsed.y);
                                var texto = formatear(valor, ctx.dataset.formato, false);
                                if (circular) {
                                    var total = ctx.dataset.data.reduce(function (a, b) { return a + b; }, 0);
                                    return ctx.label + ": " + texto + (total ? " (" + Math.round(valor * 100 / total) + "%)" : "");
                                }
                                return ctx.dataset.label + ": " + texto;
                            }
                        }
                    }
                }
            }
        };
    }

    function iniciar() {
        if (!window.Chart) {
            return;
        }
        Chart.defaults.font.family = getComputedStyle(document.body).fontFamily;
        Chart.defaults.color = "#6c757d";
        document.querySelectorAll("canvas[data-grafico]").forEach(function (canvas) {
            new Chart(canvas, configurar(JSON.parse(canvas.getAttribute("data-grafico"))));
        });
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", iniciar);
    } else {
        iniciar();
    }
})();
