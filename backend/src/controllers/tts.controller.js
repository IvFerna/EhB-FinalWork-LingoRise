const piperService = require("../services/tts/piper.service");

exports.generateSpeech = async (req, res) => {
	try {
		const { text, language } = req.body;

		if (!text || !language) {
			return res.status(400).json({
				error: "Missing text or language",
			});
		}

		const result = await piperService.generateSpeech(text, language);

		return res.json({
			audioPath: result.audioPath,
			fileName: result.fileName,
		});
	} catch (error) {
		console.error(error);

		return res.status(500).json({
			error: "Failed to generate speech",
		});
	}
};
