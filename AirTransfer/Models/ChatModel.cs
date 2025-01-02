﻿namespace AirTransfer.Models;

public enum ChatMessageType
{
    User,
    Bot
}

public class ChatModel
{
  /// <summary>
        /// Gets or sets the unique identifier for the prompt.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The unique identifier for the conversation.
        /// </summary>
        public Guid? ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the prompt text.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of response strings.
        /// </summary>
        public List<string> ResponseStrings { get; set; } = new List<string>();

        /// <summary>
        /// Response is complete.
        /// </summary>
        public bool IsComplete { get; set; } = false;

        /// <summary>
        /// Response is successful.
        /// </summary>
        public bool Success { get; private set; } = false;

        /// <summary>
        /// Occurs when the response changes.
        /// </summary>
        public event EventHandler? ResponseChanged;

        /// <summary>
        /// Occurs when the response is completed.
        /// </summary>
        public event EventHandler? ResponseCompleted;

        /// <summary>
        /// Full response text.
        /// </summary>
        public string Resonse
        {
            get
            {
                var fullText = string.Join("", ResponseStrings);
                return fullText;
            }
        }

        /// <summary>
        /// Adds a response string to the <see cref="ResponseStrings"/> list and raises the <see cref="ResponseChanged"/> event.
        /// </summary>
        /// <param name="responseString">The response string to add.</param>
        public void AddResponseString(string responseString)
        {
            ResponseStrings.Add(responseString);
            ResponseChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Marks the response as complete and raises the <see cref="ResponseCompleted"/> event.
        /// </summary>
        public void CompleteResponse(bool success)
        {
            IsComplete = true;
            Success = success;
            ResponseCompleted?.Invoke(this, EventArgs.Empty);
        }
}