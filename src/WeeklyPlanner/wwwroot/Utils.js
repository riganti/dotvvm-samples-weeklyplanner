Date.prototype.getDayOfYear = function () {
    var yearStart = new Date(this.getFullYear(), 0, 1);
    return Math.ceil(((this - yearStart) / 86400000) + 1);
};