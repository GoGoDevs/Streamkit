function Bitbar(canvasId, titleId, countId, title, color, fillColor, currentBits, maxBits, fillAreaColor, progressImage) {
    this.title = title;
    this.currentBits = currentBits;
    this.maxBits = maxBits;
    this.canvas = document.getElementById(canvasId);
    this.canvasContext = this.canvas.getContext('2d');
    this.originalImage = new Image();
    this.titleElement = document.getElementById(titleId);
    this.countElement = document.getElementById(countId);
    this.updateTitle = function(title) {
        this.title = title;
        this.renderTitle();
    };
    this.updateColor = function(color) {
        this.color = this.hexToColor(color);
        this.render();
    };
    this.updateFillColor = function(fillColor) {
        this.fillColor = this.hexToColor(fillColor);
        this.render();
    };
    this.updateFillAreaColor = function(fillAreaColor) {
        this.fillAreaColor = this.hexToColor(fillAreaColor);
        this.findOffsets();
        this.render();
    };
    this.updateCurrentBitCount = function(currentBits) {
        this.currentBits = currentBits;
        this.render();
    };
    this.updateMaxBitCount = function(maxBits) {
        if (maxBits < this.currentBits) {
            this.currentBits = maxBits;
        }
        this.maxBits = maxBits;
        this.render();
    };
    this.getCurrentBitPercentage = function() {
        return this.currentBits / this.maxBits;
    };
    this.renderTitle = function() {
        this.titleElement.style.color = this.color.hex;
        this.titleElement.innerText = this.title;
    };
    this.renderBar = function() {
        let imageData = this.canvasContext.getImageData(0, 0, this.originalImage.width, this.originalImage.height);
        let barWidth = imageData.width - this.leftBarOffset - this.rightBarOffset;
        let barCutoff = Math.round(this.leftBarOffset + barWidth * this.getCurrentBitPercentage());
        
        for (let i = 0; i < imageData.height; i++) {
            for (let j = 0; j < imageData.width; j++) {
                let start = (i * (imageData.width * 4)) + (j * 4);

                if (imageData.data[start] === this.fillAreaColor.R && imageData.data[start+1] === this.fillAreaColor.G && imageData.data[start+2] === this.fillAreaColor.B && imageData.data[start+3] > this.opacityThreshold) {
                    if (j < barCutoff) {
                        imageData.data[start] = this.color.R;
                        imageData.data[start+1] = this.color.G;
                        imageData.data[start+2] = this.color.B;
                    } else {
                        imageData.data[start] = this.fillColor.R;
                        imageData.data[start+1] = this.fillColor.G;
                        imageData.data[start+2] = this.fillColor.B;
                    }
                }
            }
        }

        this.canvasContext.putImageData(imageData, 0, 0);
    };
    this.renderBitCount = function() {
        this.countElement.style.color = this.color.hex;
        this.countElement.innerText = this.currentBits + ' / ' + this.maxBits;
    };
    this.renderOriginalImage = function() {
        let x = 0,
            y = 0,
            xI = this.originalImage.width,
            yI = this.originalImage.height;

        this.canvas.width = xI;
        this.canvas.height = yI;
        this.canvasContext.drawImage(this.originalImage, x, y, xI, yI, x, y, xI, yI);
    };
    this.render = function() {
        this.renderOriginalImage();
        this.renderTitle();
        this.renderBar();
        this.renderBitCount();
    };
    this.hexToColor = function(color) {
        let colorString = color.replace('#', '');

        if (colorString.length !== 3 && colorString.length !== 6) {
            throw 'This color is not a hex color';
        }

        if (colorString.length === 3) {
            colorString = colorString[0] + colorString[0] + colorString[1] + colorString[1] + colorString[2] + colorString[2];
        }

        let R = parseInt(colorString.substr(0, 2), 16);
        let G = parseInt(colorString.substr(2, 2), 16);
        let B = parseInt(colorString.substr(4, 2), 16);

        return {R, G, B, hex: '#' + colorString};
    };
    this.findOffsets = function() {
        this.renderOriginalImage();
        let imageData = this.canvasContext.getImageData(0, 0, this.originalImage.width, this.originalImage.height);
        let leftOffset = imageData.width;
        let rightOffset = 0;
        
        for (let i = 0; i < imageData.height; i++) {
            for (let j = 0; j < imageData.width; j++) {
                let start = (i * (imageData.width * 4)) + (j * 4);

                if (imageData.data[start] === this.fillAreaColor.R && imageData.data[start+1] === this.fillAreaColor.G && imageData.data[start+2] === this.fillAreaColor.B && imageData.data[start+3] > this.opacityThreshold) {
                    if (leftOffset > j) {
                        leftOffset = j;
                    }
                    
                    if (rightOffset < j) {
                        rightOffset = j;
                    }
                }
            }
        }
        
        //console.log('Left: ' + leftOffset);
        //console.log('Right: ' + (imageData.width - rightOffset));
        
        this.leftBarOffset = leftOffset;
        this.rightBarOffset = imageData.width - rightOffset - 1;
    };
    this.setTextWidth = function(width) {
        this.titleElement.style.width = width + 'px';
        this.countElement.style.width = width + 'px';
    };

    this.color = this.hexToColor(color);
    this.fillColor = this.hexToColor(fillColor);
    this.fillAreaColor = (typeof fillAreaColor !== 'undefined') ? this.hexToColor(fillAreaColor) : this.hexToColor('#000000');
    this.opacityThreshold = 25;
    this.leftBarOffset = 62;
    this.rightBarOffset = 7;

    this.originalImage.onload = function() {
        this.setTextWidth(this.originalImage.width);
        this.findOffsets();
        this.render();
    }.bind(this);

    this.originalImage.src = 'data:image/png;base64,' + progressImage;
}