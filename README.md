Notes and possible use cases:
- It is possible to check if the subtitle language is the original one or if it is a translation
- It is possible to request special kinds of subtitles, using the OnDemand service (captioned and audio described), this request feature does not seem to be available in the Web API


 VideoConversion solution is composed of the following components:
1) AmaraVideoClient: 
	A .dll library that can communicate with the Amara REST API given a valid username and API key. 
	The library has 3 main API methods:
		a) RequestVideoSubtitle: Given a valid video url and desired language, it will first check if the video has already been uploaded in Amara.
		If it has it will check if the there is a subtitle in the desired language. If found it will return the message that the subtitle exists, otherwise 
		it will request a subtitle translation. If the video does not exist it will upload the video to Amara and return the message that it has been submitted.
		All return messages are accompanied by the internal Amara videoId.
		b) GetVideoSubtitle: Given a valid videoId it will return the subtitle data as a byte array, along with any other relevant information
		c) GetVideoInfo: Given a valid videoId it will return all the information that Amara has about the given videoId
		d) (Not implemented yet) Optionally the videoId may be associated to a subtitling team.
		e) (Not implemented yet) The .dll will periodically check for outstanding subtitling requests and serve the result of completed translations either by email or
			other means of information relaying (may become a separate server side application)

2) VideoConversion Web Application:
	Is represented in this solution as the Robobraille.Web.API solution. It may refer to any other application that implements the AmaraVideoClient
	The solution has the following proprieties:
	a) Given a video url it will forward it to Amara and store relevant information for future processing and referencing
	b) Given a video file it will upload it to the web app server and forward a url link to Amara (under disclaimer that the user agrees to share his video with 3rd parties for subtitling)
	c) Given a document containing a video, it will extract the video file and proceed from b). Once the subtitle is created it will be merged with the document and presented to the client
	d) Optionally the video subtitling request may be shared on social media such as Facebook and Twitter for faster community response and help
