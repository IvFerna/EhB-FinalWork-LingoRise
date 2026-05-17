const { exec } = require("child_process");
const path = require("path");
const fs = require("fs");
const sanitize = require("sanitize-filename");

const STORAGE_PATH = path.join(__dirname, "../../storage/tts");

const PIPER_PATH = path.join(__dirname, "../../../../tools/piper/piper.exe");

const MODEL_PATH = path.join(
	__dirname,
	"../../../../tools/piper/models/es_ES-sharvard-medium.onnx",
);

function normalizeText(text) {
	return sanitize(text.toLowerCase().trim().replace(/\s+/g, "_"));
}

exports.generateSpeech = async (text, language) => {
	const normalized = normalizeText(text);

	const fileName = `${language}_${normalized}.wav`;

	const outputPath = path.join(STORAGE_PATH, fileName);

	// CACHE CHECK
	if (fs.existsSync(outputPath)) {
		return `/storage/tts/${fileName}`;
	}

	return new Promise((resolve, reject) => {
		const command =
			`echo "${text}" | "${PIPER_PATH}" ` +
			`--model "${MODEL_PATH}" ` +
			`--output_file "${outputPath}"`;

		console.log(PIPER_PATH);
		console.log(MODEL_PATH);
		console.log(outputPath);

		exec(command, (error) => {
			if (error) {
				return reject(error);
			}

			resolve(`/storage/tts/${fileName}`);
		});
	});
};
