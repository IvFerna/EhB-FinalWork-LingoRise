const { exec } = require("child_process");
const path = require("path");
const fs = require("fs");
const sanitize = require("sanitize-filename");

const STORAGE_PATH = path.join(__dirname, "../../storage/tts");

// const PIPER_PATH = path.join(__dirname, "../../../../tools/piper/piper.exe");
const PIPER_PATH = process.env.PIPER_PATH || path.join(__dirname, "../../../../tools/piper/piper");

const MODEL_PATH =
	process.env.PIPER_MODEL_PATH ||
	path.join(__dirname, "../../../../tools/piper/models/es_ES-sharvard-medium.onnx");

function normalizeText(text) {
	return sanitize(
		text
			.normalize("NFD")
			.replace(/[\u0300-\u036f]/g, "")
			.replace(/[^\w\s]/gi, "")
			.toLowerCase()
			.trim()
			.replace(/\s+/g, "_"),
	);
}

exports.generateSpeech = async (text, language) => {
	const normalized = normalizeText(text);
	const fileName = `${language}_${normalized}.wav`;
	const outputPath = path.join(STORAGE_PATH, fileName);

	// Ensure storage dir exists (important on first run)
	fs.mkdirSync(STORAGE_PATH, { recursive: true });

	if (fs.existsSync(outputPath)) {
		return { audioPath: `/storage/tts/${fileName}`, fileName };
	}

	return new Promise((resolve, reject) => {
		// Use single quotes inside the echo to avoid shell injection
		const safeText = text.replace(/'/g, "'\\''");
		const command =
			`echo '${safeText}' | "${PIPER_PATH}" ` +
			`--model "${MODEL_PATH}" ` +
			`--output_file "${outputPath}"`;

		exec(command, (error, stdout, stderr) => {
			if (error) {
				console.error("Piper error:", stderr);
				return reject(error);
			}
			resolve({ audioPath: `/storage/tts/${fileName}`, fileName });
		});
	});
};
