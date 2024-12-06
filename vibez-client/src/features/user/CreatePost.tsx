import React, { useState } from 'react';
import './createPost.css';
import { createPost } from '../../api/createPost.ts';

type CreatePostProps = {
  token: string;
  onPostCreated: () => void; // Callback to refresh posts after creation
  onClose: () => void; // Close the modal
};

export const CreatePost: React.FC<CreatePostProps> = ({ token, onPostCreated, onClose }) => {
  const [content, setContent] = useState('');
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleSubmit = async () => {
    if (!content.trim()) {
      setError('Content is required.');
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      const result = await createPost({ content, imageUrl: selectedFile }, token);
      if (result.success) {
        console.log('Post created:', result.data);
        onPostCreated(); // Notify parent to refresh posts
        onClose(); // Close the modal
      } else {
        setError(result.message || 'Failed to create post.');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An unexpected error occurred.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="create-post-overlay">
      <div className="create-post-box">
        <h2>Create Post</h2>
        {error && <p className="error-message">{error}</p>}
        <label>
          Content:
          <textarea
            value={content}
            onChange={(e) => setContent(e.target.value)}
            className="create-post-textarea"
            placeholder="What's on your mind?"
          />
        </label>
        <label>
          Upload Image:
          <input type="file" onChange={handleFileChange} className="create-post-file-input" />
        </label>
        <div className="create-post-actions">
          <button onClick={handleSubmit} className="create-post-save-button" disabled={isSubmitting}>
            {isSubmitting ? 'Posting...' : 'Post'}
          </button>
          <button onClick={onClose} className="create-post-cancel-button" disabled={isSubmitting}>
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};
