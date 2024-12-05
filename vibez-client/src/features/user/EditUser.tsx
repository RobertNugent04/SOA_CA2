import React, { useState } from 'react';
import './userCard.css';
import { updateProfileRequest } from '../../api/updateProfileRequest.ts';

type EditUserProps = {
  fullName: string;
  bio: string | null;
  token: string;
  onSave: (updatedData: { fullName: string; bio: string; profilePicture?: File | null }) => void;
  onClose: () => void;
};

export const EditUser: React.FC<EditUserProps> = ({ fullName, bio, token, onSave, onClose }) => {
  const [updatedFullName, setUpdatedFullName] = useState(fullName);
  const [updatedBio, setUpdatedBio] = useState(bio || '');
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleSave = async () => {
    setIsSaving(true);
    setError(null);

    try {
      const result = await updateProfileRequest(
        {
          fullName: updatedFullName,
          bio: updatedBio,
          profilePicture: selectedFile,
        },
        token
      );

      if (result.success) {
        onSave({
          fullName: updatedFullName,
          bio: updatedBio,
          profilePicture: selectedFile,
        });
        onClose();
      } else {
        setError(result.message || 'Failed to update profile.');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Something went wrong.');
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="edit-user-overlay">
      <div className="edit-user-box">
        <h2>Edit Profile</h2>
        {error && <p className="error-message">{error}</p>}
        <label>
          Full Name:
          <input
            type="text"
            value={updatedFullName}
            onChange={(e) => setUpdatedFullName(e.target.value)}
            className="edit-user-input"
          />
        </label>
        <label>
          Bio:
          <textarea
            value={updatedBio}
            onChange={(e) => setUpdatedBio(e.target.value)}
            className="edit-user-textarea"
          />
        </label>
        <label>
          Upload Profile Picture:
          <input type="file" onChange={handleFileChange} className="edit-user-file-input" />
        </label>
        <div className="edit-user-actions">
          <button onClick={handleSave} className="edit-user-save-button" disabled={isSaving}>
            {isSaving ? 'Saving...' : 'Save'}
          </button>
          <button onClick={onClose} className="edit-user-cancel-button" disabled={isSaving}>
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};
