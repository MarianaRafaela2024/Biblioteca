function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    const menuToggle = document.querySelector('.menu-toggle');
    
    sidebar.classList.toggle('active');
    overlay.classList.toggle('active');
    
    // Hide/show menu toggle button
    if (sidebar.classList.contains('active')) {
        menuToggle.classList.add('hidden');
    } else {
        menuToggle.classList.remove('hidden');
    }
    
    // Prevent body scroll when sidebar is open on mobile
    if (window.innerWidth <= 1023) {
        document.body.style.overflow = sidebar.classList.contains('active') ? 'hidden' : '';
    }
}

// Close sidebar when clicking on overlay
document.querySelector('.sidebar-overlay').addEventListener('click', toggleSidebar);

// Handle window resize
window.addEventListener('resize', function() {
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    const menuToggle = document.querySelector('.menu-toggle');
    
    if (window.innerWidth > 1023) {
        // Desktop view - force hide close button and reset sidebar
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
        menuToggle.classList.remove('hidden');
        document.body.style.overflow = '';
    }
});

// Auto-hide mobile menu when clicking on links
document.querySelectorAll('.sidebar-nav a').forEach(link => {
    link.addEventListener('click', function() {
        if (window.innerWidth <= 1023) {
            setTimeout(() => {
                toggleSidebar();
            }, 300);
        }
    });
});

// Form validation and enhancement
document.addEventListener('DOMContentLoaded', function() {
    // Add loading states to buttons
    const buttons = document.querySelectorAll('button[type="submit"]');
    buttons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Add loading state
            const originalText = button.textContent;
            button.textContent = 'Processando...';
            button.disabled = true;
            
            // Simulate API call
            setTimeout(() => {
                button.textContent = originalText;
                button.disabled = false;
            }, 2000);
        });
    });

    // Auto-resize textarea-like inputs
    const inputs = document.querySelectorAll('input[type="text"]');
    inputs.forEach(input => {
        input.addEventListener('input', function() {
            // Auto-capitalize names
            if (this.id === 'nome' || this.id === 'sobrenome' || this.id === 'upnome' || this.id === 'upsobrenome') {
                this.value = this.value.replace(/\b\w/g, l => l.toUpperCase());
            }
            
           
        });
    });

    // Form submission prevention for demo
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
        });
    });
});



// Notification system
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.textContent = message;
    
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: ${type === 'success' ? '#52c41a' : type === 'error' ? '#C41E3A' : '#2196F3'};
        color: white;
        padding: 15px 20px;
        border-radius: 8px;
        box-shadow: 0 4px 15px rgba(0,0,0,0.2);
        z-index: 10000;
        animation: slideInRight 0.3s ease;
        font-weight: 500;
        max-width: 300px;
    `;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.style.animation = 'slideOutRight 0.3s ease';
        setTimeout(() => {
            document.body.removeChild(notification);
        }, 300);
    }, 3000);
}

// Add notification animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideInRight {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    @keyframes slideOutRight {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);

/*cadLivro*/
// Enhanced functionality for details elements - making entire details clickable
document.addEventListener('DOMContentLoaded', function() {
    
    // Add click enhancement to all details elements
    const detailsElements = document.querySelectorAll('details');
    
    detailsElements.forEach(details => {
        const summary = details.querySelector('summary');
        
        // Make the entire details element clickable (excluding form inputs)
        details.addEventListener('click', function(e) {
            // Don't trigger if clicking on form inputs, buttons, or other interactive elements
            if (e.target.tagName === 'INPUT' || 
                e.target.tagName === 'SELECT' || 
                e.target.tagName === 'TEXTAREA' || 
                e.target.tagName === 'BUTTON' ||
                e.target.type === 'submit' ||
                e.target.type === 'reset' ||
                e.target.closest('input') ||
                e.target.closest('select') ||
                e.target.closest('textarea') ||
                e.target.closest('button')) {
                return;
            }
            
            // Toggle the details element
            e.preventDefault();
            this.open = !this.open;
            
            // Add visual feedback
            const summary = this.querySelector('summary');
            if (summary) {
                summary.style.transform = 'scale(0.98)';
                setTimeout(() => {
                    summary.style.transform = 'scale(1)';
                }, 150);
            }
        });
        
        // Enhanced visual feedback on summary click
        summary.addEventListener('click', function(e) {
            // Add click animation
            this.style.transform = 'scale(0.98)';
            setTimeout(() => {
                this.style.transform = 'scale(1)';
            }, 150);
        });
        
        // Add keyboard navigation
        summary.addEventListener('keydown', function(e) {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                details.open = !details.open;
                
                // Trigger click animation
                this.style.transform = 'scale(0.98)';
                setTimeout(() => {
                    this.style.transform = 'scale(1)';
                }, 150);
            }
        });
        
        // Add smooth animation when opening/closing
        details.addEventListener('toggle', function() {
            if (this.open) {
                // Animate opening
                const content = this.querySelector(':not(summary)');
                if (content) {
                    this.style.overflow = 'hidden';
                    const height = content.scrollHeight;
                    content.style.maxHeight = '0px';
                    content.style.opacity = '0';
                    
                    setTimeout(() => {
                        content.style.transition = 'max-height 0.3s ease, opacity 0.3s ease';
                        content.style.maxHeight = height + 'px';
                        content.style.opacity = '1';
                        
                        setTimeout(() => {
                            content.style.maxHeight = 'none';
                            this.style.overflow = 'visible';
                        }, 300);
                    }, 10);
                }
            }
        });
        
        // Add hover effect to make it clear the entire area is clickable
        details.addEventListener('mouseenter', function() {
            if (!this.querySelector(':hover:is(input, select, textarea, button)')) {
                this.style.cursor = 'pointer';
                this.style.transform = 'translateY(-1px)';
                this.style.boxShadow = '0 6px 20px rgba(0, 0, 0, 0.12)';
            }
        });
        
        details.addEventListener('mouseleave', function() {
            this.style.cursor = 'default';
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '0 4px 15px rgba(0, 0, 0, 0.08)';
        });
        
        // Update cursor when hovering over form elements
        const formElements = details.querySelectorAll('input, select, textarea, button');
        formElements.forEach(element => {
            element.addEventListener('mouseenter', function() {
                details.style.cursor = 'default';
                details.style.transform = 'translateY(0)';
                details.style.boxShadow = '0 4px 15px rgba(0, 0, 0, 0.08)';
            });
        });
    });
    
    // Add "Expand All" / "Collapse All" buttons functionality
    //addControlButtons();
});